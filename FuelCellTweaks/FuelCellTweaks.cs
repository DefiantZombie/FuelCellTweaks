using System.Linq;
using UnityEngine;

namespace FuelCellTweaks
{
    public class FuelCellTweaks : PartModule
    {
        private const string FieldName = "FillLimit";

        [KSPField(advancedTweakable = true, isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = FieldName), UI_FloatRange(minValue = 0.1f, maxValue = 1.0f, stepIncrement = 0.05f, scene = UI_Scene.All, affectSymCounterparts = UI_Scene.All)]
        public float FillLimit = 0.95f;

        private ModuleResourceConverter _converterModule;

        private BaseField _fillLimitField;

        public override void OnStartFinished(StartState state)
        {
            base.OnStartFinished(state);

            _converterModule = part.Modules.GetModule<ModuleResourceConverter>();
            if (_converterModule == null)
            {
                Debug.LogError("[FCT] Converter module not found.");
                return;
            }

            _converterModule.FillAmount = FillLimit;

            _fillLimitField = Fields.Cast<BaseField>().FirstOrDefault(f => f.name == FieldName);
            if (_fillLimitField == null)
            {
                Debug.LogError("[FCT] FillLimit field not found.");
                return;
            }

            _fillLimitField.OnValueModified += FillLimitChanged;
        }

        public void FillLimitChanged(object o)
        {
            _converterModule.FillAmount = (float)o;
        }

        public void OnDestroy()
        {
            if (_fillLimitField != null)
                _fillLimitField.OnValueModified -= FillLimitChanged;
        }
    }
}
