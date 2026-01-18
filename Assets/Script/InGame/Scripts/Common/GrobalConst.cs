namespace Common.Const {
    public static class GrobalConst {
        public const int CELL_SIZE = 2;

        public readonly static int TILEMAP_MARGIN = 5;
        public readonly static int MINMAP_MARGIN = 2;
    }

    public static class PlayerConst {
        public readonly static int PLAYER_VIEW_SIDE = 10;
        public readonly static int SUPPORT_AREA_SIDE = 5;
        public readonly static int MIASMA_DAMAGE = 5;
        public readonly static float AUTO_HEAL_RATE = 0.01f;
        public readonly static int ENERGY_DEFAULT_VALUE = 100;
        public readonly static int ENERGY_MAX_VALUE = 999;
        public readonly static int ENERGY_MIN_VALUE = 0;
    }

    public static class DungeonConst {
        public readonly static int DUNGEON_DEFAULT_SIZE = 30;
        public readonly static int DUNGEON_FLOOR_INCREMENT = 5;
        public readonly static int DUNGEON_STAGE_INCREMENT = 5;

        public readonly static int ENEMY_DEFAULT_COUNT = 7;
        public readonly static int ENEMY_COUNT_FLOOR_INCREMENT = 4;
        public readonly static int ENEMY_COUNT_STAGE_INCREMENT = 3;

        public readonly static int ENEMY_DEFAULT_LEVEL = 1;
        public readonly static int ENEMY_LEVEL_FLOOR_INCREMENT = 5;
        public readonly static int ENEMY_LEVEL_STAGE_INCREMENT = 30;

        public readonly static int STAGE_PER_CYCLE = 3;
        public readonly static int WORLD_PER_CYCLE = 3;
        public readonly static int ENEMY_SPAWN_INTERVAL = 5;
        public readonly static int ENEMY_ENERGY_CHANGE_INTERVAL = 5;
    }

    public static class AnimationConst {
        public readonly static float SKILL_DURATION = 0.1f;
        public readonly static float HIT_DURATION = 0.125f;
        public readonly static float EFFECT_DURATION = 0.5f;
        public readonly static float MOVE_DURATION = 0.125f;
        public readonly static float DEATH_DURATION = 0.25f;
        public readonly static float CHANGE_PLAYER_DURATION = 0.125f;
    }
}
