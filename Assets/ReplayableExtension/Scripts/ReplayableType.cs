namespace ReplayableExtension
{
    public static class ReplayableType
    {

        public const string RE_INSTANTIATE = "Instantiate";

        public const string RE_END = "End";

        public const string RE_CUSTOMEVENT = "CutomEvent";

        public const string RE_DESTORY = "Destory";

        public const string RE_ACTIVE = "Active";

        public const string RE_PARENT = "Parent";

        public const string RE_POSITION = "Position";

        public const string RE_LOCAL_POSITION = "LocalPosition";

        public const string RE_EULER_ANGLES = "EulerAngles";

        public const string RE_LOCAL_EULER_ANGLES = "LoaclEulerAngles";

        public const string RE_SCALE = "Scale";

        public const string RE_MATERIAL_FLOAT = "MaterialFloat";

        public const string RE_MATERIAL_COLOR = "MaterialColor";

        public const string RE_MATERIAL_VECTOR = "MaterialVector";

        public const string RE_ANIMATOR_INTEGER = "AnimatorInteger";

        public const string RE_ANIMATOR_FLOAT = "AnimatorFloat";

        public const string RE_ANIMATOR_BOOL = "AnimatorBool";

        public const string RE_ANIMATOR_TRIGGER = "AnimatorTrigger";

        public const string RE_ANIMATOR_RESET_TRIGGER = "AnimatorResetTrigger";

        public const string RE_ANIMATOR_PLAY = "AnimatorPlay";

        public const string RE_ANIMATOR_SPEED = "AnimatorSpeed";

        public const string RE_ANIMATOR_CROSS_FADE = "AnimatorCrossFade";

        public const string RE_ANIMATION_PLAY = "AnimationPlay";

        public const string RE_ANIMATION_STOP = "AnimationStop";

        public const string RE_ANIMATION_REWIND = "AnimationRewind";

        public static bool AllowCompress(string type)
        {
            switch (type)
            {
                case RE_PARENT:
                case RE_POSITION:
                case RE_LOCAL_POSITION:
                case RE_EULER_ANGLES:
                case RE_LOCAL_EULER_ANGLES:
                case RE_SCALE:
                case RE_ANIMATOR_INTEGER:
                case RE_ANIMATOR_FLOAT:
                case RE_ANIMATOR_BOOL:
                case RE_ANIMATOR_TRIGGER:
                case RE_ANIMATOR_RESET_TRIGGER:
                case RE_ANIMATOR_PLAY:
                case RE_ANIMATOR_SPEED:
                case RE_ANIMATOR_CROSS_FADE:
                case RE_ANIMATION_PLAY:
                case RE_ANIMATION_STOP:
                case RE_ANIMATION_REWIND:
                    return true;
                default:
                    return false;
            }
        }
    }
}
