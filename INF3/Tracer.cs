using System;
using System.Runtime.InteropServices;
using InfinityScript;

namespace INF3
{
    public static class Tracer
    {
        private static readonly IntPtr LocationalTracePointer = new IntPtr(0x48A2C0);
        private static G_LocationalTrace _locationalTraceFunction = null;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private delegate void G_LocationalTrace(ref Trace trace, ref Vector3 start, ref Vector3 end, int ignoreEntNum, int clipMask, int unk);

        public static Trace LocationalTrace(Vector3 start, Vector3 end, int ignoreEntNum = 2047, int clipMask = 0x806831)
        {
            Trace trace = new Trace();
            GetLocationalTraceFunction()(ref trace, ref start, ref end, ignoreEntNum, clipMask, 0);

            return trace;
        }

        public static Vector3 LocationalTraceFromEye(this Entity e, float maxDistance)
        {
            if (!e.IsPlayer)
            {
                throw new Exception("Entity is not player");
            }
            var headPosition = e.Call<Vector3>("gettagorigin", "tag_eye");
            var forwardAngles = Call<Vector3>("anglestoforward", e.Call<Vector3>("getplayerangles"));
            var traceEndPosition = headPosition + forwardAngles * maxDistance; // far to the forward towards crosshair
            Trace trace = LocationalTrace(headPosition, traceEndPosition);
            return trace.GetHitPosition(headPosition, traceEndPosition);
        }

        public static Vector3 PhysicsTrace(Vector3 start, Vector3 end)
        {
            return Call<Vector3>("physicstracenormal", start, end);
        }

        public static Vector3 PhysicsTraceFromEye(this Entity e, float maxDistance = 10000)
        {
            if (!e.IsPlayer)
            {
                throw new Exception("Entity is not player");
            }
            var headPosition = e.Call<Vector3>("gettagorigin", "tag_eye"); // props to kenny
            var forwardAngles = Call<Vector3>("anglestoforward", e.Call<Vector3>("getplayerangles"));
            var traceEndPosition = headPosition + forwardAngles * maxDistance; // far to the forward towards crosshair

            return PhysicsTrace(headPosition, traceEndPosition);
        }

        public static Vector3 PlayerPhysicsTrace(Vector3 start, Vector3 end)
        {
            return Call<Vector3>("playerphysicstrace", start, end);
        }

        private static G_LocationalTrace GetLocationalTraceFunction()
        {
            if (_locationalTraceFunction == null)
            {
                _locationalTraceFunction = (G_LocationalTrace)Marshal.GetDelegateForFunctionPointer(LocationalTracePointer, typeof(G_LocationalTrace));
            }
            return _locationalTraceFunction;
        }

        private static T Call<T>(string name, params Parameter[] args)
        {
            Function.SetEntRef(-1);
            return Function.Call<T>(name, args);
        }
    }

    [StructLayout(LayoutKind.Sequential, Size = 44)]
    public struct Trace
    {
        public float Fraction;
        public Vector3 Normal;
        public int SurfaceFlags;
        public int Contents;
        [MarshalAs(UnmanagedType.LPStr)]
        public string Material;
        public TraceHitType HitType;
        public short HitId;
        public short ModelIndex;
        public short PartName;
        public short PartGroup;
        public bool AllSolid;
        public bool StartSolid;
        public bool Walkable;
        public bool HitPartition;

        public Vector3 GetHitPosition(Vector3 start, Vector3 end)
        {
            return start + (end - start) * Fraction;
        }
    }

    public enum TraceHitType : int
    {
        None,
        Entity,
        DynamicEntityModel,
        DynamicEntityBrush,
        DynamicEntityUnknown
    }
}