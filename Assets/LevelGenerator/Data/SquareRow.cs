using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace LEVELGENERATOR.DATA {
    [System.Serializable]
    public class SquareRow {
        [field: SerializeField, ReadOnly(true)] private readonly string name = "Row";
        public List<SquareStates> rowElements = new List<SquareStates>();

        private string GetName() => $"Name: {name}\nRow ({rowElements.Count})"; 
    }
}