using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting.Server;
using MudBlazor;

namespace StockFishBlazorChess.Components.Dialogs
{
	public partial class PromotionDialog
	{
		[CascadingParameter]
		private MudDialogInstance? mudDialog { get; set; }

		private void promotionClick(char pieceChar)
		{
			mudDialog!.Close(DialogResult.Ok(pieceChar));
		}
		private void cancelDialog()
		{
			mudDialog!.Cancel();
		}
	}
}
