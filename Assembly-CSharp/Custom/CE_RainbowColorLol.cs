﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class CE_RainbowColorLol : MonoBehaviour
{
    public Renderer MyRend;

    public float colmul;

    void Start()
    {
        MyRend = transform.gameObject.GetComponent<Renderer>();
    }

    public void Cease()
    {
        Destroy(this);
    }


    void Update()
    {
        MyRend.material.SetColor("_BodyColor", Color.Lerp(Color.HSVToRGB(Mathf.Repeat(Time.time / 10, 1f), 1f, 1f),Palette.Brown,colmul));
        MyRend.material.SetColor("_BackColor", Color.Lerp(Color.HSVToRGB(Mathf.Repeat(Time.time / 10, 1f), 1f, 1f) * Color.gray,Palette.Brown,colmul));
    }

}

public class CE_RainbowColorNPS : MonoBehaviour
{
    public SpriteRenderer MyRend;

    void Start()
    {
        MyRend = transform.gameObject.GetComponent<SpriteRenderer>();
    }

    public void Cease()
    {
        Destroy(this);
    }


    void Update()
    {
        MyRend.color = Color.HSVToRGB(Mathf.Repeat(Time.time / 10, 1f), 1f, 1f);
    }

}
