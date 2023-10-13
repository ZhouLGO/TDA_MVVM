using System.ComponentModel;
using MVVM;

namespace UI.MVVM.Models
{
    public class Charactor_Model : IModel
    {
        public Charactor_Model(CharactorType initCharactor = CharactorType.HK416)
        {
            CurrentCharactor = initCharactor;
        }
        
        public event PropertyChangedEventHandler M_PropertyChanged;

        public void NotifyModelPropertyChanged(string propertyName)
        {
            M_PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(propertyName));
        }

        private CharactorType currentCharactor;

        public CharactorType CurrentCharactor
        {
            get => currentCharactor;
            set
            {
                if (currentCharactor != value)
                {
                    currentCharactor = value;
                    NotifyModelPropertyChanged(nameof(CurrentCharactor));
                }
            }
        }
    
    }
}
