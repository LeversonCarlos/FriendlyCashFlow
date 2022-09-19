export declare interface ICommand<P, R> {
   Handle(value: P): Promise<R>;
}
