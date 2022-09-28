using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PopupTextPool : MonoBehaviour
{
    public static PopupTextPool instance;
    private void Awake()
    {
        instance = this;
    }

    public PopupText3D pfPopupText;
    public int initialCapacity = 75;
    public int maxCapacity = 200;
    public ObjectPool<PopupText3D> pool;

    private void Start()
    {
        pool = new ObjectPool<PopupText3D>(() => // Create function
        {
            PopupText3D p = Instantiate(pfPopupText, this.transform);
            return p;
        }, popup => // get function
        {
            popup.gameObject.SetActive(true);
        }, popup => // release funtion
        {
            popup.gameObject.SetActive(false);
        }, popup => // destroy function
        {
            Destroy(popup.gameObject);
        }, true, initialCapacity, maxCapacity);
    }

    public PopupText3D GetPopupText3D()
    {
        return pool.Get();
    }

    public void ReleasePopupText3D(PopupText3D obj)
    {
        pool.Release(obj);
    }

}
