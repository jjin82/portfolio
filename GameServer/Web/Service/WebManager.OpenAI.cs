
using G2F;
using System.Collections.Concurrent;

public partial class WebManager : BaseManager<WebManager>
{
    public int ChatCount()
    {
        return _chatCount;
    }

    private int _chatCount = 0;
}
