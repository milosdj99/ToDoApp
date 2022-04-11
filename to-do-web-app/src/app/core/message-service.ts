import { Injectable } from "@angular/core";
import { Observable, Subject } from "rxjs";
import { ToDoItem } from "../models/to-do-item.model";
import { ToDoList } from "../models/to-do-list.model";
import { ToDoApiService } from "./to-do-api.service";


@Injectable({providedIn: 'root'})
export class MessageService{

    private subject = new Subject<any>();
    public $subject = this.subject.asObservable();

    private subjectSearch = new Subject<any>();
    public $subjectSearch = this.subjectSearch.asObservable();

    constructor(private api: ToDoApiService){ }

    sendCreateListMessage() {
        this.api.addList(new ToDoList()).subscribe(data => {
            this.subject.next(data);
        })
    }


    sendSearchLists(criteria: string){
        this.api.getListsBySearch(criteria).subscribe(data => {
            this.subjectSearch.next(data);
        });
    }
}