using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace ALICE_PROJECT.LEVELGENERATOR.DATA {
    [Serializable]
    public class LevelRow {
        [field: SerializeField, ReadOnly(true)] private readonly string name = "Row";
        public List<CellType> rowElements = new List<CellType>();

        private string GetName() => $"Name: {name}\nRow ({rowElements.Count})";
    }
}