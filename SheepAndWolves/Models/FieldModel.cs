using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using SheepAndWolves.Data;
using SheepAndWolves.ViewModels.Base;
using static SheepAndWolves.Data.Color;

namespace SheepAndWolves.Models
{
    internal class FieldModel : ViewModel
    {
        #region Background Color

        private string backgroundColor;
        public string BackgroundColor { get => backgroundColor; set => Set(ref backgroundColor, value); }

        #endregion

        private string selectedBackgroundColor;
        public string SelectedBackgroundColor
        {
            get
            {
                UpdateBackground();
                return selectedBackgroundColor;
            }
            set => Set(ref selectedBackgroundColor, value);

        }
        public void UpdateTexture()
        {
            if (PieceColor != Color.Piece.Empty)
            {
                TexturePath = "../../Images/" + PieceColor.ToString() + ".png";
            }
            else
            {
                TexturePath = ImagesConstsPaths.Empty;
            }
        }
        private Color.Piece pieceColor = Color.Piece.Empty;
        public Color.Piece PieceColor { get => pieceColor; set { Set(ref pieceColor, value); UpdateTexture(); }}

        #region Selected
        private bool selected;
        public bool Selected
        {
            get => selected; set
            {
                Set(ref selected, value);
                UpdateBackground();
            }
        }
        #endregion

        public void UpdateBackground()
        {
            SelectedBackgroundColor = selected ? "LightGray" : BackgroundColor;

        }

        private string texturePath = ImagesConstsPaths.Empty;
        public string TexturePath { get => texturePath; set => Set(ref texturePath, value); }

        #region I
        private int i;
        public int I { get => i; set => Set(ref i, value); }
        #endregion

        #region J
        private int j;
        public int J { get => j; set => Set(ref j, value); }
        #endregion
    }
}
