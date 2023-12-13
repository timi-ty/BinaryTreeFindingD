using System.Collections;
using UnityEditor;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public TreeNode _prefabNode;
    public int _treeSize = 7;
    public Vector2 _spacing;
    
    public void BuildBalancedTree()
    {
        if (_treeSize <= 0) return;

        if (!Mathf.IsPowerOfTwo(_treeSize + 1))
        {
            Debug.LogWarning("This tree may have fewer nodes than specified or may not be balanced.");
        }
        
        TreeNode rootNode = Instantiate(_prefabNode);
        
        rootNode.name = $"Root ({_treeSize.ToString()})";
        
        BuildBalancedTree(_treeSize, rootNode, _spacing.x);
    }

    private void BuildBalancedTree(int nodeCount, TreeNode root, float xSpace)
    {
        nodeCount--;
        
        if(!root) return;
        if (nodeCount <= 0) return;
        
        int nextDepthNodeCount = nodeCount / 2;

        TreeNode leftChild = Instantiate(_prefabNode);
        leftChild.name = $"Left ({nextDepthNodeCount.ToString()})";
        var leftChildTransform = leftChild.transform;
        leftChildTransform.parent = root.transform;
        leftChildTransform.localPosition = new Vector3(-xSpace, -_spacing.y, 0);
        BuildBalancedTree(nextDepthNodeCount, leftChild, xSpace / 2.0f);
        
        if(nodeCount >= 2)
        {
            TreeNode rightChild = Instantiate(_prefabNode);
            rightChild.name = $"Right ({nextDepthNodeCount.ToString()})";
            var rightChildTransform = rightChild.transform;
            rightChildTransform.parent = root.transform;
            rightChildTransform.localPosition = new Vector3(xSpace, -_spacing.y, 0);
            BuildBalancedTree(nextDepthNodeCount, rightChild, xSpace / 2.0f);
        }
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
