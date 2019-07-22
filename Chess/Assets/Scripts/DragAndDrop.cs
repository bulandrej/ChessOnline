using UnityEngine;

namespace Assets.Scripts
{
    class DragAndDrop
    {
        State state;
        // хранилище объекта, который тащим:
        GameObject item;
        Vector2 offset;
        Vector2 fromPosition;
        Vector2 toPosition;

        public delegate void dePickObject(Vector2 from);
        public delegate void deDropObject(Vector2 from, Vector2 to);

        dePickObject PickObject;
        deDropObject DropObject;

        public DragAndDrop(dePickObject PickObject, deDropObject DropObject)
        {
            this.PickObject = PickObject;
            this.DropObject = DropObject;
            item = null;
            state = State.none;
        }
        enum State
        {
            //Вообще, может быть 4 состояния, но мы используем только 2:
            none, // ничего
            drag // потащили
        }

        public void Action()
        {

            // switch по всем состояниям
            switch (state)
            {
                // ничего не произошло 
                case State.none:
                    if (IsMouseButtonPressed()) // если была нажата какая-либо клавиша
                        PickUp(); // тогда мы должны "схватить" объект
                    break; // заканчиваем действие

                // процесс переноса
                case State.drag:
                    if (IsMouseButtonPressed()) // если была нажата какая-либо кнопка
                        Drag(); // мы продолжаем нести объект
                    else // а если не нажата, и мы в состоянии переноса - это означает, что мы "бросили" объект, и процесс переноса закончился 
                        Drop(); // метод "бросить"
                    break; // заканчиваем действие
            }
        }

        bool IsMouseButtonPressed()
        {
            return Input.GetMouseButton(0); // 0 - номер кнопки, которая могла быть нажата
            // в данном случае - левая
        }

        void PickUp()
        { //взять("схватить") объект(фигуру)
          //т.е. - мышка была нажата, и мы что-то "схватили"
            Vector2 clickPosition = GetClickPosition();
            Transform clickedItem = GetItemAt(clickPosition);  // получаем результат работы функции GetItemAt

            if (clickedItem == null) // т.е. - если ничего не нажато
                return; // тогда мы выходим.
            // раз мы уже схватили фигуру - меняем статус (что фигура - "взятА"):
            state = State.drag; // потащили
            item = clickedItem.gameObject; // gameObject и является той фигурой, которую мы потащим.
            fromPosition = clickedItem.position;
            offset = fromPosition - clickPosition; // берём позицию, и отнимаем ту позицию, где мы щёлкнули
                                                   // *для вычитания clickedItem.position необходимо привести к типу (потому что сам clickedItem.position - это Vector3, а у нас - Vector2)
                                                   // после этого - можно складывать эти векторы
            PickObject(fromPosition);
        }

        // этот метод позволит узнать: а где же мы кликнули
        Vector2 GetClickPosition()
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        // определяем, какой объект находится в этом месте
        Transform GetItemAt(Vector2 position)
        {
            // параметры: позиция, направление (здесь - неактуально, т.к. 2D), расстояние ("радиус действия")
            RaycastHit2D[] figures = Physics2D.RaycastAll(position, position, 0.5f);
            if (figures.Length == 0)
                return null; // фигур под зоной клика нет
                             // а если не ноль - вернём первую фигуру из списка:
            return figures[0].transform; // методом проб и ошибок установлено, что вполне подойдёт экземпляр трансформ :)
                                         //  потому что - именно в нём находится всё, что нам необходимо - и координаты, и объект.
        }

        void Drag()
        {
            item.transform.position = GetClickPosition() + offset; // берём новую позицию мышки, и туда наш объект перемещаем
        }

        void Drop()
        {
            toPosition = item.transform.position;
            DropObject(fromPosition, toPosition);
            item = null;
            state = State.none;
        }
    }
}
