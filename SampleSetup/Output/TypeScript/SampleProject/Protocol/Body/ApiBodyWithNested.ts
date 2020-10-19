export class ApiBodyWithNested {
    Prop1: string = '';
    Prop2: number | null = null;

    export class Nested_1 {
        Prop_1_1: string = '';
        Prop_1_2: number | null = null;

        export class Nested_1_2 {
            Prop_1_2_1: string = '';
            Prop_1_2_2: number | null = null;
        }
    }
}
