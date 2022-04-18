using TMPro;
using UnityEngine;

namespace clicker
{
    public class Scorer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textbox;
        private int _score;

        private void OnEnable()
        {
            _textbox.SetText(_score.ToString());
        }

        public void IncreaseScore()
        {
            _score++;
            _textbox.SetText(_score.ToString());
        }
    }
}
