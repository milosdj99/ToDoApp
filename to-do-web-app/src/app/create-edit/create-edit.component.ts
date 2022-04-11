import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToDoApiService } from '../core/to-do-api.service';
import { ToDoList } from '../models/to-do-list.model';
import { FormControl, FormGroup, FormArray, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { ToDoItem } from '../models/to-do-item.model';
import { MessageService } from '../core/message-service';
import { Subscription } from 'rxjs';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';

@Component({
    selector: 'app-create-edit',
    templateUrl: './create-edit.component.html',
    styleUrls: ['./create-edit.component.css']
})
export class CreateEditComponent implements OnInit, OnDestroy {

    formGroup = new FormGroup({
        id: new FormControl(),
        title: new FormControl(),
        reminderDate: new FormControl(),
        itemsChecked: new FormArray([]),
        itemsNotChecked: new FormArray([])
    });
    subscription: Subscription;

    constructor(private api: ToDoApiService, private route: ActivatedRoute, private messageService: MessageService) {
        this.subscription = this.messageService.$subject.subscribe(list => {
            this.formGroup.controls["id"].patchValue(list.id);
        })
    }
    ngOnDestroy(): void {
        this.subscription.unsubscribe();
    }

    ngOnInit(): void {
        let id = this.route.snapshot.paramMap.get('id');

        if (id) {
            this.api.getToDoListById(id).subscribe(data => {
                this.patchData(data);
                this.checkedControls.sort((a, b) => (a.get('position')?.value - b.get('position')?.value));
                this.notCheckedControls.sort((a, b) => (a.get('position')?.value - b.get('position')?.value));
            });
        }
    }


    get checkedFormArray(){
        return this.formGroup.get('itemsChecked') as FormArray;
    }

    get notCheckedFormArray(){
        return this.formGroup.get('itemsNotChecked') as FormArray;
    }

    get checkedControls(){
        return this.checkedFormArray.controls as Array<FormGroup>;
    }

    get notCheckedControls(){
        return this.notCheckedFormArray.controls as Array<FormGroup>;
    }

    private readonly patchData = (toDoList: ToDoList) => {
        this.formGroup.controls['title'].patchValue(toDoList.title);
        this.formGroup.controls['reminderDate'].patchValue(toDoList.reminderDate);
        this.formGroup.controls['id'].patchValue(toDoList.id);
        toDoList.items.forEach(item => {
            if(item.checked == true){
                this.checkedFormArray.push(this.CreateItemFormGroup(item));
            } else {
                this.notCheckedFormArray.push(this.CreateItemFormGroup(item));
            }
        }
        );
    }

    private readonly CreateItemFormGroup = (toDoItem: ToDoItem) =>
        new FormGroup({
            id: new FormControl(toDoItem.id),
            content: new FormControl(toDoItem.content),
            checked: new FormControl(toDoItem.checked),
            position: new FormControl(toDoItem.position)
        });

    public deleteItem(id: string) {
        this.checkedFormArray.removeAt(this.checkedFormArray.controls.findIndex(x => x.get("id")?.value == id));
        this.notCheckedFormArray.removeAt(this.notCheckedFormArray.controls.findIndex(x => x.get("id")?.value == id));
    }

    applyChanges() {
        let listId = this.formGroup.get('id')?.value;

        if (listId) {
            let list = new ToDoList();
            list.id = listId;
            list.title = this.formGroup.get("title")?.value;
            list.reminderDate = this.formGroup.get("reminderDate")?.value;
            list.items = [];

            this.api.modifyList(listId, list).subscribe();
        } else {
            let list = new ToDoList();
            list.title = this.formGroup.get("title")?.value;
            list.reminderDate = this.formGroup.get("reminderDate")?.value; 
            this.api.addList(list).subscribe(data => {
                this.formGroup.controls['id'].patchValue(data.id);
            });
        }
    }

    addItem(newItem: ToDoItem) {
        this.notCheckedFormArray.push(this.CreateItemFormGroup(newItem));
    }

    dropNotChecked(event: CdkDragDrop<FormGroup[]>) {
        moveItemInArray(this.notCheckedControls, event.previousIndex, event.currentIndex);   
        let itemId = this.notCheckedControls[event.currentIndex].get('id')?.value;
        this.api.modifyItemPosition(this.formGroup.get('id')?.value, itemId, event.currentIndex).subscribe(); 
    }

    dropChecked(event: CdkDragDrop<FormGroup[]>) {
        moveItemInArray(this.checkedControls, event.previousIndex, event.currentIndex);
        let itemId = this.checkedControls[event.currentIndex].get('id')?.value;
        this.api.modifyItemPosition(this.formGroup.get('id')?.value, itemId, event.currentIndex).subscribe();       
    }

    checkChange(item: FormGroup){
        if(item.get('checked')?.value == true){
            this.notCheckedFormArray.removeAt(this.notCheckedFormArray.controls.findIndex(x => x.get("id")?.value == item.get('id')?.value));
            this.checkedFormArray.push(item);
        } else {
            this.checkedFormArray.removeAt(this.checkedFormArray.controls.findIndex(x => x.get("id")?.value == item.get('id')?.value));
            this.notCheckedFormArray.push(item);
        }
        this.checkedControls.sort((a, b) => (a.get('position')?.value - b.get('position')?.value));
        this.notCheckedControls.sort((a, b) => (a.get('position')?.value - b.get('position')?.value));
    }    
}


