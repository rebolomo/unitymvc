using UnityEngine;
using System.Collections;

public class lsAnim : NewMono 
{
    public void easyInOutTo(Vector3 pos, Quaternion rot, float animLength)
    {
        AnimationClip clip       = new AnimationClip();
        AnimationCurve curvePosX = AnimationCurve.EaseInOut(0, trans.localPosition.x, animLength, pos.x);
        AnimationCurve curvePosY = AnimationCurve.EaseInOut(0, trans.localPosition.y, animLength, pos.y);
        AnimationCurve curvePosZ = AnimationCurve.EaseInOut(0, trans.localPosition.z, animLength, pos.z);
        AnimationCurve curveRotX = AnimationCurve.EaseInOut(0, trans.localRotation.x, animLength, rot.x);
        AnimationCurve curveRotY = AnimationCurve.EaseInOut(0, trans.localRotation.y, animLength, rot.y);
        AnimationCurve curveRotZ = AnimationCurve.EaseInOut(0, trans.localRotation.z, animLength, rot.z);
        AnimationCurve curveRotW = AnimationCurve.EaseInOut(0, trans.localRotation.w, animLength, rot.w);
        System.Type transType    = typeof(Transform);
        clip.SetCurve("", transType, LSConst.LOCALPOS_X, curvePosX);
        clip.SetCurve("", transType, LSConst.LOCALPOS_Y, curvePosY);
        clip.SetCurve("", transType, LSConst.LOCALPOS_Z, curvePosZ);
        clip.SetCurve("", transType, LSConst.LOCALROT_X, curveRotX);
        clip.SetCurve("", transType, LSConst.LOCALROT_Y, curveRotY);
        clip.SetCurve("", transType, LSConst.LOCALROT_Z, curveRotZ);
        clip.SetCurve("", transType, LSConst.LOCALROT_W, curveRotW);
        clip.wrapMode = WrapMode.Clamp;
        const string clipName = "ls_move";
        anim.AddClip(clip, clipName);
        anim.Play(clipName);
    }

    public void easyInOutFromTo(Vector3 fromPos, Vector3 toPos, Quaternion fromRot, Quaternion toRot, float animLength)
    {
        trans.localPosition = fromPos;
        trans.localRotation = fromRot;
        easyInOutTo(toPos, toRot, animLength);
    }

}

