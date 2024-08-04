using Unit.GameScene.Units.Cards.Abstract;

namespace Unit.GameScene.Units.Panels.Interfaces
{
    public interface ICardPool
    {
        public CardView Get();
        public void Release(CardView blockView);
    }
}