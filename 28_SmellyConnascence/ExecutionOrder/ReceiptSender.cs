namespace SmellyConnascenceKata.ExecutionOrder;

// Connascence of Execution Order: Archive() is only correct if
// SendToCustomer() has already run, but nothing in the type or method
// signatures expresses that -- the caller has to know the right order
// from tribal knowledge, not from the code.
public class ReceiptSender
{
    private bool _sent;

    public void SendToCustomer(string receiptId)
    {
        Console.WriteLine($"Emailing receipt {receiptId} to customer");
        _sent = true;
    }

    public void Archive(string receiptId)
    {
        if (!_sent)
        {
            Console.WriteLine($"Warning: archiving {receiptId} before it was sent");
        }

        Console.WriteLine($"Archiving receipt {receiptId}");
    }
}
