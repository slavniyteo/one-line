using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections;
using OneLine;

public class ForeachTest {

	[Test]
	public void Action() {
		var enumerable = new int[]{1,2,3,4,5};
		int i = 0;
		enumerable.ForEachExceptLast(x => i++);

		Assert.AreEqual(enumerable.Length - 1, i);
	}

	[Test]
	public void LastAction() {
		var enumerable = new int[]{1,2,3,4,5};
		int i = 0;
		enumerable.ForEachExceptLast(x => {}, x => i = x);

		Assert.AreEqual(enumerable[enumerable.Length - 1], i);
	}
}
