using System.Collections;
using UnityEngine;

namespace Transitions
{
	public class TransitionController : MonoBehaviour
	{
		[SerializeField]
		string _parameterName;
		[field: SerializeField]
		public Animator Animator { get; private set; }

		bool IsExecuting { get; set; } = false;

		public void Done()
		{
			IsExecuting = false;
		}

		public void Execute(AsyncOperation operation)
		{
			if(IsExecuting)
				return;
			
			StartCoroutine(ExecutionCoroutine(operation));
		}

		IEnumerator ExecutionCoroutine(AsyncOperation operation)
		{
			operation.allowSceneActivation = false;
			Execute();
			IsExecuting = true;
			yield return new WaitForSeconds(1f);
			operation.allowSceneActivation = true;
			yield return new WaitWhile(() => IsExecuting);
			Execute();
		}
		
		void Execute()
		{
			Animator.SetTrigger(_parameterName);
		}
		
#if UNITY_EDITOR
		void Reset()
		{
			Animator = GetComponent<Animator>();
		}
#endif
	}
}