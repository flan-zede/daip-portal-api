namespace api.Wrappers
{
    public class Page
    {
        public int Index { get; set; }
        public int Size { get; set; }

        public Page(int index, int size) {
            Index = index;
            Size = size;
        }
    }

}
