using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Unit : Entity
{
    public BoxCollider2D col;
    public List<Vector3> movePath;
    public List<Vector3> linePoints;
    public List<Vector3> tmpMovePath;
    public float speed;
    public float speedModifier;
    public bool selected = false;
    public LineRenderer movePathLine;
    public bool drawingMovePath = false;

    public SpriteRenderer highlight;

    public void Start()
    {
        movePathLine.transform.SetParent(null);
        movePathLine.transform.localScale = Vector3.one;
    }

    public void UpdateUnit()
    {
        if (selected) highlight.transform.localScale = Vector3.one;
        else highlight.transform.localScale = Vector3.zero;
        if (Input.GetMouseButtonDown(0) && selected && !drawingMovePath)
        {
            selected = false;
        }

        if (col.OverlapPoint(M.cam.ScreenToWorldPoint(Input.mousePosition)))
        {
            if (Input.GetMouseButtonDown(0) && M.netgame.units.Contains(this))
            {
                selected = !selected;
                if (selected)
                {
                    linePoints = new List<Vector3>(movePath);
                    linePoints.Insert(0, transform.position);
                }
            }
        }

        if (selected && linePoints.Count > 0)
        {
            linePoints[0] = transform.position;
            movePathLine.positionCount = linePoints.Count;
            movePathLine.SetPositions(linePoints.ToArray());
        } else
        {
            movePathLine.positionCount = 0;
        }

        if (Input.GetMouseButtonDown(1) && selected)
        {
            if (!drawingMovePath)
            {
                drawingMovePath = true;
                tmpMovePath.Clear();
            }
            tmpMovePath.Add(M.cam.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10));
            linePoints = new List<Vector3>(tmpMovePath);
            linePoints.Insert(0, transform.position);
        }
        if (Input.GetMouseButtonDown(0) && selected && drawingMovePath)
        {
            movePath = new List<Vector3>(tmpMovePath);
            List<float> orderData = new List<float>();
            foreach (Vector3 pos in movePath)
            {
                orderData.Add(pos.x);
                orderData.Add(pos.y);
            }
            M.netgame.SendOrderRpc("move", orderData.ToArray(), index);

            drawingMovePath = false;
        }
        
        if (movePath.Count > 0)
        {
            transform.position += (movePath[0] - transform.position).normalized * speed * speedModifier * Time.deltaTime;
            if (Vector2.Distance(transform.position, movePath[0]) < speed * speedModifier * Time.deltaTime * 2)
            {
                transform.position = movePath[0];
                movePath.RemoveAt(0);

                linePoints = new List<Vector3>(movePath);
                linePoints.Insert(0, transform.position);
            }
        }
    }
}
