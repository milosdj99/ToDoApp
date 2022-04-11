import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { MatButtonModule } from "@angular/material/button";
import { MatCardModule } from "@angular/material/card";
import { MatCheckboxModule } from "@angular/material/checkbox";
import { MatListModule } from "@angular/material/list";
import { MatToolbarModule } from "@angular/material/toolbar";
import { BrowserModule } from "@angular/platform-browser";
import { CreateEditComponent } from "./create-edit.component";
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from "@angular/material/core";
import { ToDoItemComponent } from "./to-do-item/to-do-item.component";
import { CommonModule } from "@angular/common";
import { DragDropModule } from '@angular/cdk/drag-drop';


@NgModule({
    declarations: [
        CreateEditComponent,
        ToDoItemComponent
    ],
    imports: [
        BrowserModule,
        MatButtonModule,
        MatToolbarModule,
        MatListModule,
        MatCardModule,
        MatCheckboxModule,
        ReactiveFormsModule,
        MatInputModule,
        MatDatepickerModule,
        MatNativeDateModule,
        CommonModule,
        DragDropModule,
    ]
})
export class CreateEditModule { }