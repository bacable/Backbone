using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.Actions
{
    public enum ActionType
    {
        MoveTo,
        MoveBy,
        ScaleTo,
        FadeAlpha,
        RotateX,
        RotateY,
        RotateZ,
        Wait,
        Group,
        Sequence,
        ChangeModel,
        RemoveFromParent,
        PlaySound,
        GameEventEnded,
        MenuEvent,
        RegisterGroupEvent,
        CompleteGroupEvent,
        ChangeScreen
    }
}
