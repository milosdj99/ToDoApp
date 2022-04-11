import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { isNgTemplate } from '@angular/compiler';
import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, AbstractControl, FormArray } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { MessageService } from 'src/app/core/message-service';
import { ToDoApiService } from 'src/app/core/to-do-api.service';
import { ToDoItem } from 'src/app/models/to-do-item.model';

@Component({
    selector: 'app-to-do-item',
    templateUrl: './to-do-item.component.html',
    styleUrls: ['./to-do-item.component.css']
})
export class ToDoItemComponent {
    @Input() listId: string = "";
    @Output() deleteEvent = new EventEmitter();
    @Output() addEvent = new EventEmitter();
    @Output() checkChangeEvent = new EventEmitter();
    @Input() formGroup = new FormGroup({
        id: new FormControl(),
        content: new FormControl(),
        checked: new FormControl(),
        position: new FormControl()
    });

    subscription: Subscription;

    constructor(private api: ToDoApiService, private route: ActivatedRoute, private messageService: MessageService) {
        this.subscription = messageService.$subject.subscribe(data => {
            this.listId = data.id;
            this.addItem();
        });
    }

    deleteItem() {
        let itemId = this.formGroup.controls["id"].value;

        if (this.listId && itemId) {
            this.api.deleteItem(this.listId, itemId).subscribe();
        }

        this.deleteEvent.emit(itemId);
    }

    addItem() {
        if (this.formGroup.get('content')?.value != '') {

            let newItem = new ToDoItem();
            newItem.toDoListId = this.listId;
            newItem.content = this.formGroup.get('content')?.value;
            newItem.checked = false;

            this.api.addItem(this.listId, newItem).subscribe(data => {
                newItem.id = data.id;
                this.addEvent.emit(newItem);
            });

            this.formGroup.controls['content'].patchValue('');
        }
    }

    applyChanges() {
        let item = this.getDataFromForm();

        if (this.listId) {
            if (this.formGroup.get("id")?.value == null) {
                this.addItem();
            } else {
                this.api.modifyItem(this.listId, item).subscribe();
            }
        }else{
            this.messageService.sendCreateListMessage();
        }  
        this.checkChangeEvent.emit(this.formGroup);           
    }



    private getDataFromForm() {
        let item = new ToDoItem();
        item.id = this.formGroup.get("id")?.value;
        item.content = this.formGroup.get("content")?.value;
        item.checked = this.formGroup.get("checked")?.value;
        item.position = this.formGroup.get("position")?.value;
        item.toDoListId = this.listId;

        return item;
    }
}
