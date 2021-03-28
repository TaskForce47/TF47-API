using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace TF47_API.Database.Models.GameServer.AAR
{
    public interface IObject { };

    public record Unit(string Name, string Side, string Group, string CurrentWeapon, bool IsIncapacitated, bool IsAi) : IObject;

    public record Vehicle(string Name, string Type, float Damage) : IObject;

    public record UpdatePos(float[] Pos, float[] Vector, IObject Object);

    public record ProjectileFired(IObject Shooter, string Projectile, float[] Origin, float[] Vector);

    public record Died(IObject Object);

    public record Damaged(IObject Object, float OldValue, float NewValue);

    public record Healed(IObject Healer, IObject Target);

    //public record PositionData(string Name, string Side, int[] position)
}
