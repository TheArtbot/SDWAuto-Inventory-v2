using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaleEntryScript : MonoBehaviour
{
    public SalesHistoryUI UiHandler;

    public void ExecuteSelection()
    {
        UiHandler.SelectSaleEntry(this.gameObject);
    }
}
