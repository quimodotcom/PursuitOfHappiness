using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Happiness : MonoBehaviour
{
	public happinessChangeType changeType;

	private SpriteRenderer renderer;
	private CircleCollider2D collider;
	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag("Player")) {
			if(changeType == happinessChangeType.increase)
            {
				Game.Instance.IncreaseHappiness();
				gameObject.SetActive(false);
			}
            else
            {
				Game.Instance.DecreaseHappiness();
				gameObject.SetActive(false);
			}
		}
	}
}

public enum happinessChangeType
{
	increase,
	decrease
}
