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
        if (col.bounds.Contains(M.cam.ScreenToWorldPoint(Input.mousePosition)))
        {
            if (Input.GetMouseButtonDown(0))
            {
                selected = !selected;
            }
        }

        if (Input.GetMouseButtonDown(1) && selected) targetPos = M.cam.ScreenToWorldPoint(Input.mousePosition);
        
        if (targetPos != transform.position)
        {
            transform.position += (targetPos - transform.position).normalized * speed * speedModifier;
            if (Vector2.Distance(transform.position, targetPos) < speed / 10f) transform.position = targetPos;
        }
    }
}
