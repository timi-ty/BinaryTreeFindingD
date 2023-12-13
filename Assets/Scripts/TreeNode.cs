using System;
using TMPro;
using UnityEngine;

public class TreeNode : MonoBehaviour
{
        public LineRenderer _edges;
        public GameObject _callout;
        public TextMeshPro _calloutText;
        public bool _isDeviant;
        public SpriteRenderer _spriteRenderer;
        public Color _deviantColor;
        
        [HideInInspector]
        public bool _foundDeviant;
        [HideInInspector]
        public TreeNode _left;
        [HideInInspector]
        public TreeNode _right;

        private void Awake()
        {
                _spriteRenderer = GetComponent<SpriteRenderer>();
                if (_isDeviant) _spriteRenderer.color = _deviantColor;

                UpdateEdgesColors();
        }

        public GameObject PreVisitNode()
        {
                _callout.SetActive(true);

                if (_left || _right)
                {
                        _calloutText.text = "(↓↓) My two children.\nFind the deviant node and bring me its color.";
                }
                else
                {
                        _calloutText.text = "(↓↓) My chil...\nI have no children.";
                }

                return _callout;
        }

        public GameObject PostVisitNode()
        {
                _callout.SetActive(true);
                
                _foundDeviant = _isDeviant || (_left && _left._foundDeviant) || (_right && _right._foundDeviant);

                //Propagate the deviant color up the chain that found it.
                if (_left && _left._foundDeviant)
                {
                        _spriteRenderer.color = _left._spriteRenderer.color;
                }
                else if(_right && _right._foundDeviant)
                {
                        _spriteRenderer.color = _right._spriteRenderer.color;
                }
                        
                if (_isDeviant)
                {
                        _calloutText.text = $"(↑) I am the deviant!\nMy color is {_spriteRenderer.color.ToString()}!";
                }
                else if(_foundDeviant)
                {
                        _calloutText.text = $"(↑) I found the deviant.\nIt's color is {_spriteRenderer.color.ToString()}.";
                }
                else
                {
                        _calloutText.text = "(↑) I found no deviant.";
                }
                
                UpdateEdgesColors();

                return _callout;
        }

        private void UpdateEdgesColors()
        {
                if (!_edges) return;
                var colorGradient = _edges.colorGradient;
                var colorKeys = colorGradient.colorKeys;
                colorKeys[1].color = _spriteRenderer.color;
                if (_left) colorKeys[0].color = _left._spriteRenderer.color;
                if (_right) colorKeys[2].color = _right._spriteRenderer.color;
                colorGradient.colorKeys = colorKeys;
                _edges.colorGradient = colorGradient;
        }
}