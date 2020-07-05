using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TF47_Api.Database;

namespace TF47_Api.Services
{
    public class SquadXmlService
    {
        private readonly ILogger<SquadXmlService> _logger;
        private readonly IServiceProvider _serviceProvider;
        //private readonly Tf47DatabaseContext _database;
        private readonly string _path;
        private readonly string _amakePath;

        public SquadXmlService(ILogger<SquadXmlService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            //_database = database;
            _path = Path.Combine(Environment.CurrentDirectory, "wwwroot/squadxml").Replace("\\", "/");
            _amakePath = Path.Combine(Environment.CurrentDirectory, "Files/armake").Replace("\\", "/");

            if (Directory.Exists(_path))
                _logger.LogInformation($"wwwroot squadxml path found successful");
            else
            {
                _logger.LogWarning("wwwroot squadxml path not found, creating new one...");
                try
                {
                    Directory.CreateDirectory(_path);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Cannot create squadxml path {ex.Message}");
                    throw;
                }

                RegenerateSquadXmls().Wait();
            }
        }

        public void DeletePicture(Tf47GadgetSquad squad)
        {
            var picturePath = Path.Combine(_path, squad.SquadNick, "picture.paa");
            try
            {
                if (File.Exists(picturePath))
                    File.Delete(picturePath);
            }
            catch (Exception e)
            {
                _logger.LogError($"Could not delete squad picture: {e.Message}");
            }
        }


        public async Task GenerateSquadXml(uint squadId)
        {
            using var scope = _serviceProvider.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<Tf47DatabaseContext>();
            var squad = await database.Tf47GadgetSquad
                .Include(x => x.Tf47GadgetSquadUser)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == squadId);
            if (squad == null)
            {
                _logger.LogError($"Cannot find any squad with id {squadId}");
                return;
            }

            if (!Directory.Exists(Path.Combine(_path, squad.SquadNick)))
            {
                _logger.LogInformation($"No wwwroot path found for squad {squad.SquadName}. Creating new!");
                Directory.CreateDirectory(Path.Combine(_path, squad.SquadNick));
            }

            var data = new List<string>();
            data.Add("<?xml version=\"1.0\"?>");
            data.Add("<?xml-stylesheet type=\"text/xsl\" href=\"../../squadxml/squad.xsl\" ?>");
            data.Add("<!DOCTYPE squad SYSTEM \"squad.dtd\">");
            data.Add($"<squad nick=\"{squad.SquadNick}\">");
            data.Add($"\t<name>{squad.SquadName}</name>");
            data.Add($"\t<email>{squad.SquadEmail}</email>");
            data.Add($"\t<web>{squad.SquadWeb}</web>");
            if (squad.SquadHasPicture)
            {
                data.Add($"\t<picture>logo.paa</picture>");
            }
            data.Add($"\t<title>{squad.SquadTitle}</title>");
            foreach (var tf47GadgetSquadUser in squad.Tf47GadgetSquadUser)
            {
                var player =
                    await database.Tf47ServerPlayers.FirstOrDefaultAsync(x =>
                        x.PlayerUid == tf47GadgetSquadUser.User.PlayerUid);

                data.Add(player == null
                    ? $"\t<member id=\"\" nick=\"{"N/A"}\">"
                    : $"\t<member id=\"{tf47GadgetSquadUser.User.PlayerUid}\" nick=\"{player.PlayerName}\">");
                data.Add($"\t\t<name>{tf47GadgetSquadUser.UserSquadName}</name>");
                data.Add($"\t\t<email>{tf47GadgetSquadUser.UserSquadEmail}</email>");
                data.Add($"\t\t<icq>N/A</icq>");
                data.Add($"\t\t<remark>{tf47GadgetSquadUser.UserSquadRemark}</remark>");
                data.Add($"\t</member>");
            }
            data.Add("</squad>");

            var squadXmlPath = Path.Combine(_path, squad.SquadNick, "squad.xml").Replace("\\", "/");
            var squadDtdPath = Path.Combine(_path, squad.SquadNick, "squad.dtd").Replace("\\", "/");

            if (File.Exists(squadXmlPath))
                File.Delete(squadXmlPath);
            if (File.Exists(squadDtdPath))
                File.Delete(squadDtdPath);

            await File.WriteAllLinesAsync(squadXmlPath, data.ToArray(), Encoding.UTF8);

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
            await File.WriteAllLinesAsync(squadDtdPath, dtd, CancellationToken.None);
            _logger.LogInformation($"Create squad.xml file for unit {squad.SquadName}.");
        }

        public void DeleteSquadXml(Tf47GadgetSquad squad)
        {
            var xmlPath = Path.Combine(_path, squad.SquadNick);
            if (Directory.Exists(xmlPath))
            {
                _logger.LogInformation($"Deleting squad xml {squad.SquadName}");
                try
                {
                    Directory.Delete(xmlPath);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Cannot delete squad xml {ex.Message}");
                }
            }
        }

        public async Task RegenerateSquadXmls()
        {
            _logger.LogInformation($"Regenerating squad xmls...");
            using var scope = _serviceProvider.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<Tf47DatabaseContext>();
            var squads = database.Tf47GadgetSquad.Where(x => x.Id > 0).AsNoTracking();
            foreach (var squad in squads)
            {
                _logger.LogInformation($"Regenerating {squad.SquadName}..");
                await GenerateSquadXml(squad.Id);
            }
        }

        public async Task<bool> CreatePicture(IFormFile data, Tf47GadgetSquad squad)
        {
            var picturePath = Path.Combine(_path, squad.SquadNick, "logo.png").Replace("\\","/");
            var paaPath = Path.Combine(_path, squad.SquadNick, "logo.paa").Replace("\\", "/");

            if (File.Exists(picturePath))
                File.Delete(picturePath);

            if(File.Exists(paaPath))
                File.Delete(paaPath);

            await using var stream = new FileStream(picturePath, FileMode.CreateNew);
            await data.CopyToAsync(stream);

            _logger.LogInformation("Trying to convert image to paa");
            using var image = Image.FromFile(picturePath);
            if (image.Height == image.Width)
            {
                try
                {
                    var process = Process.Start(_amakePath, $"img2paa -f {picturePath} {paaPath}");
                    process.WaitForExit(60 * 1000);
                    if (File.Exists(paaPath)) return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Failed converting image to paa: {ex.Message}");
                }
            }
            else
            {
                _logger.LogWarning("Image must have the same height and width!");
                File.Delete(picturePath);
            }

            return false;
        }
    }
}