using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
    public TreeNode _rootNode;
    
    private bool _goNext;

    private readonly List<GameObject> _activeCallOuts = new();

    private IEnumerator ExecuteSequence(TreeNode root)
    {
        if(!root) yield break;
        
        //Wait until the sequence is ready to progress.
        yield return new WaitUntil(() => _goNext);

        //Pause the sequence after every progression.
        _goNext = false;

        //Disable all currently active callouts every time the sequence progresses.
        foreach (GameObject callOut in _activeCallOuts)
        {
            callOut.SetActive(false);
        }
        
        _activeCallOuts.Clear();
        
        _activeCallOuts.Add(root.PreVisitNode());

        yield return ExecuteSequence(root._left);
        yield return ExecuteSequence(root._right);
        
        //Wait until the sequence is ready to progress.
        yield return new WaitUntil(() => _goNext);

        //Pause the sequence after every progression.
        _goNext = false;
        
        _activeCallOuts.Add(root.PostVisitNode());
    }

    private void Start()
    {
        StartCoroutine(ExecuteSequence(_rootNode));
    }

    private void Update()
    {
        //Progresses the sequence every time the mouse button is clicked.
        if (Input.GetMouseButtonDown(0))
        {
            _goNext = true;
        }
    }
}
