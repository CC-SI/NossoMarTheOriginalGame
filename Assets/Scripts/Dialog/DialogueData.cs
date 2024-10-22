using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Dialog
{
    /// <summary>
    /// Classe que representa os dados de diálogos, contendo uma lista de grupos de diálogos.
    /// </summary>
    [Serializable]
    public class DialogueData
    {
        public List<DialogueGroup> dialogueGroups;
    }
    
    /// <summary>
    /// Classe que representa um grupo de diálogos, incluindo um ID, nome do grupo e diálogos contidos.
    /// </summary>
    [Serializable]
    public class DialogueGroup
    {
        public string id;
        public string groupName;
        public List<Dialogue> dialogues;

        public DialogueGroup() 
        {
            dialogues = new List<Dialogue>();
        }
    }
    
    /// <summary>
    /// Classe que representa um diálogo, incluindo o nome do falante e o texto do diálogo.
    /// </summary>
    [Serializable]
    public class Dialogue
    {
        public string id;
        public string speaker;
        public string text;
        public TipoDeDialogoEnum tipoDeDialogoEnum;
        
        public Dialogue() {} 
        
        /// <summary>
        /// Construtor que permite criar um diálogo com o nome do falante e o texto.
        /// </summary>
        /// <param name="speaker">Nome do falante.</param>
        /// <param name="text">Texto do diálogo.</param>
        public Dialogue(string speaker, string text, string id)
        {
            this.speaker = speaker;
            this.text = text;
            this.id = id;
        }
    }
}