import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToDoApiService } from '../core/to-do-api.service';
import { ToDoItem } from '../models/to-do-item.model';
import { ToDoList } from '../models/to-do-list.model';

@Component({
  selector: 'app-to-do-list-share',
  templateUrl: './to-do-list-share.component.html',
  styleUrls: ['./to-do-list-share.component.css']
})
export class ToDoListShareComponent implements OnInit{

  list : ToDoList = new ToDoList();

  constructor(private api: ToDoApiService, private route: ActivatedRoute) {
      
  }

  ngOnInit(): void {
    let id = this.route.snapshot.paramMap.get('id');

        if (id) {
            this.api.getSharedToDoList(id).subscribe(data => {
                this.list = data;
            });
        }      
  }


}
