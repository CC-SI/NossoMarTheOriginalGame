using System;
using Assets.Scripts.Dialogos.Enum;
using UnityEngine;

namespace Assets.Scripts.Dialogos
{
    [Serializable]
    public class Dialogo
    {
        public string id;
        public string orador;
        [TextArea(2, 10)]
        public string texto;
        public TipoDialogoEnum TipoDialogoEnum;
    }
    

}