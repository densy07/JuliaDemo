using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuliaControl : MonoBehaviour
{
    public float m_scale = 1;

    private Vector2 m_downPos;
    private Vector2 m_lastPos;
    private bool m_hasMouseDown;

    private Vector2 _screenP;

    public Material m_mat;

    public bool m_doUpdate = true;

    public float _C;

    // Start is called before the first frame update
    void Start ()
    {
        m_hasMouseDown = false;
        _screenP = new Vector2 (Screen.width / 2, Screen.height / 2);
        m_mat.SetVector ("_P", new Vector4 (0, 0, 0, 0));

        var uvScale = Vector2.one;
        if (Screen.width > Screen.height)
        {
            uvScale.x = Screen.width / (float) Screen.height;
        }
        else
        {
            uvScale.y = Screen.height / (float) Screen.width;
        }

        Debug.Log (uvScale);
        m_mat.SetVector ("_uvScale", uvScale);
    }

    private void OnRenderImage (RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit (src, dest, m_mat);
    }

    // Update is called once per frame
    void Update ()
    {
        if (m_mat == null || m_doUpdate == false)
        {
            return;
        }

        if (Input.GetMouseButtonDown (0))
        {
            m_downPos = Input.mousePosition;
            m_lastPos = m_downPos;
        }

        if (Input.GetMouseButton (0))
        {
            Vector2 currentPos = Input.mousePosition;
            _screenP += (currentPos - m_lastPos) * m_scale;
            m_lastPos = currentPos;

            var pos = _screenP;

            var x = pos.x / Screen.width;
            var y = pos.y / Screen.height;

            x = (x - 0.5f) * 2;
            y = (y - 0.5f) * 2;

            m_mat.SetVector ("_P", new Vector4 (x, y, 0, 0));
        }

        var mouseDelta = Input.mouseScrollDelta;

        if (mouseDelta.y > 0)
        {
            m_scale /= 1.1f;
        }
        else if (mouseDelta.y < 0)
        {
            m_scale *= 1.1f;
        }

        m_mat.SetFloat ("_S", m_scale);
        m_mat.SetVector ("_C", new Vector4 (_C, _C));
    }
}