using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IDOElementLinearTrack<T>
{
	void Clear();
	void Add (T elem);
	bool Has (T elem);
}

public enum ElementLinearTrackCode
{
	OK,
	REMOVE_LAST,
	ALREADY_EXIST
}

public class DOElementLinearTrack<T> : IDOElementLinearTrack<T> {

	public List<T> track { get; protected set; }
	public ElementLinearTrackCode lastCode { get; protected set; }
	public T last { 
		get { return _last; }
	}

	protected T _last;
	protected T _prev;
	protected T _defvalue;

	public DOElementLinearTrack(T defvalue)
	{
		track = new List<T> ();
		Clear ();
	}

	virtual public void Clear()
	{
		track.Clear();
		_last = _defvalue; _prev = _defvalue;
	}

	/*
	 * 0 - ok, 1 - remove last, 2 - obj already exist
	 */ 
	virtual public void Add(T obj)
	{
		lastCode = ElementLinearTrackCode.OK;

		if(this._IsDef(_last)) 
		{
			this._AddToTrack(obj);
		}
		else
		{
			if(this.IsPrev(obj)) 
			{
				this._RemoveLast();
				lastCode = ElementLinearTrackCode.REMOVE_LAST;
				return;
			}
			else 
			{
				if(this.Has(obj))
				{
					lastCode = ElementLinearTrackCode.ALREADY_EXIST;
				}

				this._AddToTrack(obj);
			}
		}
	}

	virtual public bool Has (T elem)
	{
		return track.Contains (elem);
	}


	virtual protected void _AddToTrack(T obj)
	{
		if (!this._IsDef (_last))
			_prev = _last;

		_last = obj;
		track.Add (obj);
	}

	virtual protected void _RemoveLast()
	{
		track.RemoveAt (track.Count - 1);

		if (track.Count < 1)
			_last = _defvalue;
		else
			_last = track [track.Count - 1];

		if (track.Count < 2)
			_prev = _defvalue;
		else
			_prev = track [track.Count - 2];
	}

	virtual protected bool _IsDef(T obj)
	{
		return obj.Equals (_defvalue);
	}

	virtual public void RevomeLast()
	{
		this._RemoveLast ();
	}

	virtual public bool IsPrev(T obj)
	{
		if(this._IsDef(_prev)) return false;
		return obj.Equals (_prev);
	}










}
