using System.Collections;
using UnityEditor;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public TreeNode _prefabNode;
    public LineRenderer _prefabLine;
    public int _treeSize = 7;
    public Vector2 _spacing;
    private Transform _lineParent;
    
    public void BuildBalancedTree()
    {
        if (_treeSize <= 0) return;

        if (!Mathf.IsPowerOfTwo(_treeSize + 1))
        {
            Debug.LogWarning("This tree may have fewer nodes than specified or may not be balanced.");
        }
        
        TreeNode rootNode = Instantiate(_prefabNode);
        rootNode.name = $"Root ({_treeSize.ToString()})";

        _lineParent = new GameObject("Connecting Lines").transform;
        
        BuildBalancedTree(_treeSize, rootNode, _spacing.x);
    }

    private void BuildBalancedTree(int nodeCount, TreeNode root, float xSpace)
    {
        nodeCount--;
        
        if(!root) return;
        if (nodeCount <= 0) return;
        
        int nextDepthNodeCount = nodeCount / 2;

        LineRenderer connectingLine = Instantiate(_prefabLine, _lineParent, true);
        connectingLine.positionCount = 3;
        connectingLine.SetPosition(1, root.transform.position);

        root._edges = connectingLine;

        TreeNode leftChild = Instantiate(_prefabNode);
        root._left = leftChild;
        leftChild.name = $"Left ({nextDepthNodeCount.ToString()})";
        Transform leftChildTransform = leftChild.transform;
        leftChildTransform.parent = root.transform;
        leftChildTransform.localPosition = new Vector3(-xSpace, -_spacing.y, 0);
        connectingLine.SetPosition(0, leftChildTransform.position);
        BuildBalancedTree(nextDepthNodeCount, leftChild, xSpace / 2.0f);

        if (nodeCount < 2) return;
        
        TreeNode rightChild = Instantiate(_prefabNode);
        root._right = rightChild;
        rightChild.name = $"Right ({nextDepthNodeCount.ToString()})";
        Transform rightChildTransform = rightChild.transform;
        rightChildTransform.parent = root.transform;
        rightChildTransform.localPosition = new Vector3(xSpace, -_spacing.y, 0);
        connectingLine.SetPosition(2, rightChildTransform.position);
        BuildBalancedTree(nextDepthNodeCount, rightChild, xSpace / 2.0f);
    }
}

[CustomEditor(typeof(Manager))]
public class ManagerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if (GUILayout.Button("Build Balanced Tree"))
        {
            ((Manager) target).BuildBalancedTree();
        }
    }
}
