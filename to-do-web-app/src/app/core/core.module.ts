import { NgModule } from "@angular/core";
import { ToDoApiService } from "./to-do-api.service";

@NgModule({
    providers: [ToDoApiService]
})
export class CoreModule { }