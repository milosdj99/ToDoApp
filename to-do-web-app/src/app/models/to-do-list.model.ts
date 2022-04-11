import { ToDoItem } from "./to-do-item.model";

export class ToDoList {
    id: string | undefined;
    title: string | undefined;
    items = new Array<ToDoItem>();
    reminderDate: Date | undefined;
    position = 0;
    notified = false;
}