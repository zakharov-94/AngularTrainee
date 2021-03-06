import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { State, process } from '@progress/kendo-data-query';

import { GetMagazineViewItem } from '../../../models/magazineViewModel/getMagazineViewItem';
import { PostMagazineViewItem } from '../../../models/magazineViewModel/postMagazineViewItem';
import { GetMagazineViewModel } from '../../../models/magazineViewModel/getMagazineViewModel';
import { PostMagazineViewModel } from '../../../models/magazineViewModel/postMagazineViewModel';

import { AccountService } from '../../../services/account.service';
import { MagazineDataService } from '../../../services/magazine.service';

@Component({
    templateUrl: './magazine.component.html'
})
export class MagazineComponent implements OnInit {
    public magazines: GetMagazineViewItem[];
    private editedRowIndex: number;
    private editedItem: PostMagazineViewItem;
    public formGroup: FormGroup;
    public isAdmin: boolean;

    public gridState: State = {
        sort: [],
        skip: 0,
        take: 10
    };

    constructor(private magazineDataService: MagazineDataService, private accountService: AccountService) { }

    ngOnInit() {
        this.load();
        this.isAdmin = this.accountService.isAdmin();
    }

    load() {
        this.magazineDataService.getMagazines().subscribe((data: GetMagazineViewModel) => this.magazines = data.magazines);
    }

    public removeHandler({ dataItem }) {
        this.magazineDataService.deleteMagazine(dataItem.id).subscribe(data => this.load());;
    }

    public onStateChange(state: State) {
        this.gridState = state;

        this.load();
    }

    public addHandler({ sender }) {
        this.closeEditor(sender);
        this.formGroup = new FormGroup({
            'id': new FormControl({ value: 0, disabled: true }, Validators.required),
            'name': new FormControl('', Validators.required),
            'number': new FormControl(0, Validators.required),
            'dateOfPublishing': new FormControl(new Date(2000, 1, 1), Validators.required)
        });
        sender.addRow(this.formGroup);
    }

    public editHandler({ sender, rowIndex, dataItem }) {
        this.closeEditor(sender);
        this.formGroup = new FormGroup({
            'id': new FormControl({ value: dataItem.id, disabled: true }, Validators.required),
            'name': new FormControl(dataItem.name, Validators.required),
            'number': new FormControl(dataItem.number, Validators.required),
            'dateOfPublishing': new FormControl(new Date(dataItem.dateOfPublishing), Validators.required)
        });
        this.editedRowIndex = rowIndex;
        sender.editRow(rowIndex, this.formGroup);
    }

    public cancelHandler({ sender, rowIndex }) {
        this.closeEditor(sender, rowIndex);
        this.load();
    }

    public saveHandler({ sender, rowIndex, formGroup, isNew }) {
        var magazine: PostMagazineViewItem = formGroup.getRawValue();
        var oldDate = new Date(magazine.dateOfPublishing);
        magazine.dateOfPublishing = new Date(oldDate.getFullYear(), oldDate.getMonth(), oldDate.getDate(), 2, 0, 0);
        var magazineModel: PostMagazineViewModel = new PostMagazineViewModel();
        magazineModel.postMagazineViewItem = magazine;
        if (isNew) { this.magazineDataService.createMagazine(magazineModel).subscribe(data => this.load()); }
        if (!isNew) { this.magazineDataService.updateMagazine(magazineModel).subscribe(data => this.load()); }
        sender.closeRow(rowIndex);

        this.editedRowIndex = undefined;
        this.editedItem = undefined;
    }

    private closeEditor(grid, rowIndex = this.editedRowIndex) {
        grid.closeRow(rowIndex);
        this.editedRowIndex = undefined;
        this.editedItem = undefined;
    }
}
