import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ToDoApiService } from 'src/app/core/to-do-api.service';
import { ToDoList } from '../../models/to-do-list.model';
import { Router } from '@angular/router';
import { Clipboard } from '@angular/cdk/clipboard';



@Component({
  selector: 'app-to-do-preview',
  templateUrl: './to-do-preview.component.html',
  styleUrls: ['./to-do-preview.component.css']
})
export class ToDoPreviewComponent {

  @Input() toDoList = new ToDoList();

  @Output() deleteEvent = new EventEmitter();

  constructor(private api: ToDoApiService, private router: Router, private clipboard: Clipboard){ }

  deleteToDoList(id: string | undefined){
    if(id){
      this.api.deleteToDoList(id).subscribe();
      this.deleteEvent.emit(id);
    }
  }

  editToDoList(id: string | undefined){
    this.router.navigate(["/toDoLists", id]);
  }

  shareToDoList(id: string | undefined){  
      let shareId;   
      this.api.addSharedToDoList(id!).subscribe(data=>{
        shareId = data;
        this.clipboard.copy(`https://to-do-web-app.azurewebsites.net/share/${shareId}`);
      });
      
    }
  }
