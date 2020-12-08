interface View<ModelType>
{
    string Render(ModelType model);
}

interface View
{
    string Render();
}