namespace SpaceHacathon.Statistics {

    public class PlayerShipStatistics {
        public int MissileHit { get; set; }
        public int CarrierRocketHit { get; set; }
        public int StunHit { get; set; }
        public int LaserHit { get; set; }

        public int EnemyShipHit { get; set; }
        public int RamShipHit { get; set; }
        public int MineHit { get; set; }

        public int KillDroneCarrier { get; set; }
        public int KillRamShip { get; set; }
        public int KillStunShip { get; set; }
        public int KillRocketShip { get; set; }

        public int ChargeUsed { get; set; }

        public int EnergyBarrelTake { get; set; }
        public float TimeInSlowingCloud { get; set; }

        public int HealStationUse { get; set; }
        public int TotalCargoBrought { get; set; }
        public int NoSalvation { get; set; }

        public float TimeOn1Battery { get; set; }
        public float TimeOn2Battery { get; set; }
        public float TimeOn3Battery { get; set; }
        public float TimeOn4Battery { get; set; }
        public float TimeOn5Battery { get; set; }
    }

}