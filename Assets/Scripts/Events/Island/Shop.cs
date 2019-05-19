using System.Collections.Generic;
using Items;

namespace Events.Island
{
    public class Shop {
        private int height;
        private List<IItem> pool;

        public Shop(int height) {
            this.height = height;
        }
    }
}