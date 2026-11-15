using UnityEngine;

public class Unit : Entity
{
    public BoxCollider2D col;
    public Vector3 targetPos;
    public float speed;
    public float speedModifier;
    public bool selected = false;

    

    public void UpdateUnit()
    {
        if (col.OverlapPoint(M.cam.ScreenToWorldPoint(Input.mousePosition)))
        {
            if (Input.GetMouseButtonDown(0))
            {
                selected = !selected;
            }
        }

        if (Input.GetMouseButtonDown(1) && selected)
        {
            targetPos = M.cam.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = 0;
            M.netgame.SendOrderRpc("move", new float[]{targetPos.x, targetPos.y}, index);
        }
        
        if (targetPos != transform.position)
        {
            transform.position += (targetPos - transform.position).normalized * speed * speedModifier * Time.deltaTime;
            if (Vector2.Distance(transform.position, targetPos) < speed * speedModifier * Time.deltaTime * 2) transform.position = targetPos;
        }
    }
}
