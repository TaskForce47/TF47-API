using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TF47_Backend.Database;
using TF47_Backend.Database.Models.Services;
using TF47_Backend.Helper;

namespace TF47_Backend.Services.SquadManager
{
    public class SquadManagerService : ISquadManagerService
    {
        private readonly ILogger<SquadManagerService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly SemaphoreSlim _lock;
        private readonly string _squadHomePath;
        private readonly string _squadUrl;

        public SquadManagerService(
            ILogger<SquadManagerService> logger, 
            IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _lock = new SemaphoreSlim(1);
            _squadHomePath = PathCombiner.Combine(Environment.CurrentDirectory, "wwwroot", "squadxml");
            _squadUrl = $"{configuration["BaseUrl"]}/squadxml";
        }

        public async Task<bool> WriteSquadXml(long squadId, CancellationToken cancellationToken)
        {
            var squadPath = PathCombiner.Combine(_squadHomePath, squadId.ToString());
            var dtdPath = PathCombiner.Combine(squadPath, "squad.dtd");
            var xmlPath = PathCombiner.Combine(squadPath, "squad.xml");
            var logoPath = PathCombiner.Combine(squadPath, "logo.png");
            
            await _lock.WaitAsync(cancellationToken);

            using var scope = _serviceProvider.CreateScope();
            await using var database = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            var squad = await database.Squads
                .Include(x => x.SquadMembers)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.SquadId == squadId, cancellationToken: cancellationToken);

            if (squad == null)
            {
                _logger.LogWarning("Squad does not exist!");
                _lock.Release();
                return false;
            }

            if (!Directory.Exists(squadPath))
                Directory.CreateDirectory(squadPath);

            if (File.Exists(logoPath) && string.IsNullOrEmpty(squad.PictureUrl))
            {
                _logger.LogInformation("Squad {squadName}:{squadId} found new logo! Updating database",
                    squad.Name, squadId);
                squad.PictureUrl = $"{_squadUrl}/{squadId}/logo.png";
                try
                {
                    await database?.SaveChangesAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        "Failed to update squad when trying to set haspicture property for squad {squadid}: {message}",
                        squadId, ex.Message);
                    _lock.Release();
                    return false;
                }
            }

            #region DTD and XML generation

            var squadXml = new List<string>
            {
                "<?xml version=\"1.0\"?>",
                "<?xml-stylesheet type=\"text/xsl\" href=\"../squad.xsl\" ?>",
                "<!DOCTYPE squad SYSTEM \"squad.dtd\">",
                $"<squad nick=\"{squad.Nick}\">",
                $"\t<name>{squad.Name}</name>",
                $"\t<email>{squad.Mail}</email>",
                $"\t<web>{squad.Website}</web>"
            };
            if (! string.IsNullOrEmpty(squad.PictureUrl))
            {
                squadXml.Add($"\t<picture>logo.paa</picture>");
            }
            squadXml.Add($"\t<title>{squad.Title}</title>");
            foreach (var squadMember in squad.SquadMembers)
            {
                var player = await database.Players
                    .FindAsync(squadMember.User.SteamId);

                squadXml.Add(player == null
                    ? $"\t<member id=\"{squadMember.User.SteamId}\" nick=\"{squadMember.User.Username}\">"
                    : $"\t<member id=\"{squadMember.User.SteamId}\" nick=\"{player.PlayerName}\">");
                squadXml.Add($"\t\t<name>{squadMember.User.Username}</name>");
                squadXml.Add($"\t\t<email>{squadMember.Mail}</email>");
                squadXml.Add($"\t\t<icq>N/A</icq>");
                squadXml.Add($"\t\t<remark>{squadMember.Remark}</remark>");
                squadXml.Add($"\t</member>");
            }
            squadXml.Add("</squad>");
            
            string[] dtd =
            {
                "<!ELEMENT squad (name, email, web?, picture?, title?, member+)>",
                "<!ATTLIST squad nick CDATA #REQUIRED>",
                "",
                "<!ELEMENT member (name, email, icq?, remark?, picture?, steamid?)>",
                "<!ATTLIST member id CDATA #REQUIRED nick CDATA #REQUIRED>",
                "",
                "<!ELEMENT name (#PCDATA)>",
                "<!ELEMENT email (#PCDATA)>",
                "<!ELEMENT icq (#PCDATA)>",
                "<!ATTLIST icq href CDATA #IMPLIED>",
                "<!ELEMENT steamid (#PCDATA)>",
                "<!ELEMENT web (#PCDATA)>",
                "<!ELEMENT remark (#PCDATA)>",
                "<!ELEMENT picture (#PCDATA)>",
                "<!ELEMENT title (#PCDATA)>"
            };
            #endregion

            _logger.LogInformation("Updating squadxml xml files for squad {squadName}:{squadId}",
                squad.Name, squadId);
            
            if (File.Exists(dtdPath))
                File.Delete(dtdPath);
            if (File.Exists(xmlPath))
                File.Delete(xmlPath);
            
            await File.WriteAllLinesAsync(dtdPath, dtd, cancellationToken);
            await File.WriteAllLinesAsync(xmlPath, squadXml, cancellationToken);
            
            _lock.Release();
            
            _logger.LogInformation("Updating squadxml xml files for squad {squadName}:{squadId} successful",
                squad.Name, squadId);
            
            return true;
        }

        public async Task<bool> UpdateSquadImage(long squadId, Stream image, CancellationToken cancellationToken)
        {
            var squadPath = PathCombiner.Combine(_squadHomePath, squadId.ToString());
            var logoPath = PathCombiner.Combine(squadPath, "logo.png");
            
            if (!Directory.Exists(squadPath))
                Directory.CreateDirectory(squadPath);
            
            _logger.LogInformation("Updating squad logo for squad {squadId}", squadId);
            
            await using var stream = File.Create(logoPath);
            await image.CopyToAsync(stream, cancellationToken);
            
            stream.Close();
            
            var imageToPaaConverter = new ImageToPaaConverter();
            
            try
            {
                var result = await imageToPaaConverter.Convert(logoPath, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to write squad image for squadid {squadId} ! {message}", squadId, ex.Message);
                return false;
            }

        }

        public async Task<bool> DeleteSquad(long squadId, CancellationToken cancellationToken)
        {
            await _lock.WaitAsync(cancellationToken);
            
            var squadPath = PathCombiner.Combine(_squadHomePath, squadId.ToString());

            try
            {
                if (Directory.Exists(squadPath))
                    Directory.Delete(squadPath, true);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to remove existing squad folder, possible it is being accessed right now");
            }
            finally
            {
                _lock.Release();
            }
            
            return true;
        }
    }
}