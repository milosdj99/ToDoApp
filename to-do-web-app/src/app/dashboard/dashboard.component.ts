import { Component, OnDestroy, OnInit } from '@angular/core';
import { ToDoApiService } from '../core/to-do-api.service';
import { ToDoList } from '../models/to-do-list.model';
import { Router } from '@angular/router';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { AppComponent } from '../app.component';
import { MessageService } from '../core/message-service';
import { Subscription } from 'rxjs';

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit, OnDestroy{

    toDoLists = new Array<ToDoList>();

    subscription: Subscription;

    constructor(private api: ToDoApiService, private router: Router, private messages: MessageService) {    
        this.subscription = this.messages.$subjectSearch.subscribe(data => {
            this.toDoLists = data;
        });
    }
    ngOnDestroy(): void {
        this.subscription.unsubscribe();
    }

    ngOnInit(): void {
        this.api.getToDoLists().subscribe(data => {
            this.toDoLists = data;
            this.toDoLists.sort((a,b) => (a.position - b.position));
        });        
    }

    deleteList(id: string){
        this.toDoLists = this.toDoLists.filter(x => x.id !== id);
    }

    onDrop(event: CdkDragDrop<any[]>) {
        moveItemInArray(this.toDoLists, event.previousIndex, event.currentIndex);   
        let listId = this.toDoLists[event.currentIndex].id!; 
        this.api.modifyListPosition(listId, event.currentIndex).subscribe();       
    }
}
