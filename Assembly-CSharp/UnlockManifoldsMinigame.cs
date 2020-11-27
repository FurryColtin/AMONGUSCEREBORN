﻿using System;
using System.Collections;
using System.Linq;
using UnityEngine;

// Token: 0x020001AB RID: 427
public class UnlockManifoldsMinigame : Minigame
{
	// Token: 0x06000912 RID: 2322 RVA: 0x00030804 File Offset: 0x0002EA04
	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		int num = 2;
		int num2 = this.Buttons.Length / num;
		float[] array = FloatRange.SpreadToEdges(-1.7f, 1.7f, num2).ToArray<float>();
		float[] array2 = FloatRange.SpreadToEdges(-0.43f, 0.43f, num).ToArray<float>();
		SpriteRenderer[] array3 = this.Buttons.ToArray<SpriteRenderer>();
		array3.Shuffle<SpriteRenderer>();
		for (int i = 0; i < num2; i++)
		{
			for (int j = 0; j < num; j++)
			{
				int num3 = i + j * num2;
				array3[num3].transform.localPosition = new Vector3(array[i], array2[j], 0f);
			}
		}
	}

	// Token: 0x06000913 RID: 2323 RVA: 0x000308B0 File Offset: 0x0002EAB0
	public void HitButton(int idx)
	{
		if (this.MyNormTask.IsComplete)
		{
			return;
		}
		if (this.animating)
		{
			return;
		}
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(this.PressButtonSound, false, 1f).pitch = Mathf.Lerp(0.5f, 1.5f, (float)idx / 10f);
		}
		if (idx == this.buttonCounter)
		{
			this.Buttons[idx].color = Color.green;
			this.buttonCounter++;
			if (this.buttonCounter == this.Buttons.Length)
			{
				this.MyNormTask.NextStep();
				base.StartCoroutine(base.CoStartClose(0.75f));
				return;
			}
		}
		else
		{
			this.buttonCounter = 0;
			base.StartCoroutine(this.ResetAll());
		}
	}

	// Token: 0x06000914 RID: 2324 RVA: 0x00007803 File Offset: 0x00005A03
	private IEnumerator ResetAll()
	{
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(this.FailSound, false, 1f);
		}
		this.animating = true;
		for (int i = 0; i < this.Buttons.Length; i++)
		{
			this.Buttons[i].color = Color.red;
		}
		yield return new WaitForSeconds(0.25f);
		for (int j = 0; j < this.Buttons.Length; j++)
		{
			this.Buttons[j].color = Color.white;
		}
		yield return new WaitForSeconds(0.25f);
		for (int k = 0; k < this.Buttons.Length; k++)
		{
			this.Buttons[k].color = Color.red;
		}
		yield return new WaitForSeconds(0.25f);
		for (int l = 0; l < this.Buttons.Length; l++)
		{
			this.Buttons[l].color = Color.white;
		}
		this.animating = false;
		yield break;
	}

	// Token: 0x040008CD RID: 2253
	public SpriteRenderer[] Buttons;

	// Token: 0x040008CE RID: 2254
	public byte SystemId;

	// Token: 0x040008CF RID: 2255
	private int buttonCounter;

	// Token: 0x040008D0 RID: 2256
	private bool animating;

	// Token: 0x040008D1 RID: 2257
	public AudioClip PressButtonSound;

	// Token: 0x040008D2 RID: 2258
	public AudioClip FailSound;
}
