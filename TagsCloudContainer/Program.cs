﻿using System.Drawing;
using Autofac;
using TagsCloudContainer.Common;
using TagsCloudContainer.Layouters;
using TagsCloudVisualization;
using TagsCloudVisualization.Interfaces;
using TagsCloudVisualization.Visualizators;

namespace TagsCloudContainer
{
    internal class Program
    {
        private const string textFilename = "..\\..\\..\\Tags\\startTags.txt";
        private const string imageFilename = "..\\..\\..\\images\\image1.jpg";

        private static void Main(string[] args)
        {
            var container = RegisterDependencies();
            ResolveContainer(container);
        }

        private static void ResolveContainer(IContainer container)
        {
            var minSize = new SizeF(20, 10);
            var maxScale = 10;
            using (var scope = container.BeginLifetimeScope())
            {
                var reader = scope.Resolve<TagReader>();
                var parser = scope.Resolve<WordsCountParser>();
                var layouter = scope.Resolve<TagLayouter>();
                var visualizator = scope.Resolve<CloudVisualizator>();

                var text = reader.Read(textFilename);
                var wordsCount = parser.Parse(text);
                var cloud = layouter.PlaceTagsInCloud(wordsCount, minSize, maxScale);
                visualizator.Visualize(cloud);
            }
        }

        private static IContainer RegisterDependencies()
        {
            var builder = new ContainerBuilder();
            RegisterLibraryDependencies(builder);
            RegisterProjectDependencies(builder);
            
            return builder.Build();
        }

        private static void RegisterLibraryDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<Cloud>()
                .As<ICloud<ITag>>()
                .WithParameter("center", new PointF());

            builder.RegisterType<Spiral>()
                .As<ISpiral>()
                .WithParameter("center", new PointF());

            builder.RegisterType<Tag>()
                .As<ITag>();

            builder.RegisterType<Tag>();

            builder.RegisterType<CircularCloudLayouter>()
                .As<ICloudLayouter>();

            builder.RegisterType<TagsVisualizator>()
                .As<IVisualizator<ITag>>();

            builder.RegisterType<TagsVisualizatorSettings>()
                .As<IVisualizatorSettings>()
                .WithParameter("filename", imageFilename);
        }

        private static void RegisterProjectDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<FileBlobStorage>()
                .As<IBlobStorage>();

            builder.RegisterType<Serializer>()
                .As<ISerializer>();

            builder.RegisterType<TagCircularLayouter>()
                .As<TagLayouter>();

            builder.RegisterType<TagReader>().AsSelf();
            builder.RegisterType<WordsCountParser>().AsSelf();
            builder.RegisterType<CloudVisualizator>().AsSelf();
        }
    }
}