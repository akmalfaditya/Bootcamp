class MyClass : IDisposable
{
	private int[] myArray;
	private FileStream fs;
	private OfficeStream officeStream;
	private bool disposedValue;

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				myArray = null;
			}

			fs.Dispose();
			officeStream = null;
			disposedValue = true;
		}
	}
	//Safety net
	~MyClass()
	{
		// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		Dispose(disposing: false);
	}

	public void Dispose()
	{
		// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}